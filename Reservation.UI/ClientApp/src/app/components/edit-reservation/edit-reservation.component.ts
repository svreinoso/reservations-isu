import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { NotificationsService } from 'angular2-notifications';
import { ContactModel } from 'src/app/interfaces/contactModel';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-edit-reservation',
  templateUrl: './edit-reservation.component.html',
  styleUrls: ['./edit-reservation.component.scss']
})
export class EditReservationComponent implements OnInit {

  public Editor = ClassicEditor;
  public editorConfig = {

  }
  editorModel = '';
  contactForm: FormGroup;
  model: ContactModel;
  contactTypes = [
    { name: 'Home', value: 1 },
    { name: 'Office', value: 2 },
    { name: 'Mobile', value: 3 }
  ];
  timer: any;
  reservationId: 0;
  contactId: 0;

  constructor(private fb: FormBuilder, private httpService: HttpService, private route: ActivatedRoute,
    private notificationService: NotificationsService) { }

  ngOnInit(): void {
    this.loadForm();
    this.route.params.subscribe(params => {
      this.reservationId = params['reservationId'];
      if(this.reservationId && this.reservationId > 0) {
        this.httpService.get('Reservations/' + this.reservationId).subscribe((response: any) => {
          this.editReservation(response);
        });
      } 
      this.contactId = params['contactId'];
      if(this.contactId && this.contactId > 0) {
        this.httpService.get('Contacts/' + this.contactId).subscribe((response: any) => {
          this.editReservation(response);
        });
      }
    });
  }

  onEditorReady(editor) {
    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
      return {

      /**
       * Starts the upload process.
       * @returns {Promise}
       */
        upload: () => {
          return new Promise((resolve, reject) => {
            const reader = new window.FileReader();

            reader.addEventListener('load', () => {
              resolve({ default: reader.result });
            });

            reader.addEventListener('error', err => {
              reject(err);
            });

            reader.addEventListener('abort', () => {
              reject();
            });

            loader.file.then(file => {
              reader.readAsDataURL(file);
            });
          });
        }
      };
    };
  }

  loadForm() {
    this.contactForm = this.fb.group({
      id: new FormControl(0),
      name: new FormControl('', Validators.required),
      phoneNumber: new FormControl(''),
      contactType: new FormControl(0, [Validators.required, Validators.min(1)]),
      birthdate: new FormControl('', [Validators.required]),
      editorData: new FormControl('')
    });
  }

  editReservation(model : ContactModel) {
    this.contactForm.patchValue({
      id: model.id,
      name: model.name,
      phoneNumber: model.phoneNumber,
      contactType: model.contactType,
      birthdate: model.birthDate ? model.birthDate.toString().substr(0, 10) : '',
      editorData: model.editorData || ''
    });
  }

  onKeyUp(event) {
    let value = event.target.value;
    if (!value) return;

    clearTimeout(this.timer);
    this.timer = setTimeout(() => {
      this.httpService.get('Contacts/GetByName/' + value)
      .subscribe((response: any) => {
        if(response && response.name) {
          this.editReservation(response);
        }
      });
    }, 1000);
  }

  onSubmit() {

    if (this.contactForm.invalid) {
      this.markFormGroupTouched(this.contactForm)
      return;
    }

    let data = this.contactForm.value;
    data.reservationId = this.reservationId;
    data.userId = this.httpService.getUserId();
    this.httpService.post('contacts', data).subscribe(() => {
      this.notificationService.success('Saved.')
      location.href = '/';
    });
  }

  markFormGroupTouched(formGroup: FormGroup) {
    if (formGroup.controls) {
      const keys = Object.keys(formGroup.controls);
      for (let i = 0; i < keys.length; i++) {
        const control = formGroup.controls[keys[i]];
        if (control instanceof FormControl) {
          control.markAsTouched();
        } else if (control instanceof FormGroup) {
          this.markFormGroupTouched(control);
        }
      }
    }
  }

  get name() { return this.contactForm.get('name'); }
  get contactType() { return this.contactForm.get('contactType'); }
  get birthdate() { return this.contactForm.get('birthdate'); }

}
