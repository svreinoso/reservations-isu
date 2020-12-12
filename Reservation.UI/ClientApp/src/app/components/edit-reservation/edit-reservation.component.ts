import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ContactModel } from 'src/app/interfaces/contactModel';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-edit-reservation',
  templateUrl: './edit-reservation.component.html',
  styleUrls: ['./edit-reservation.component.css']
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

  constructor(private fb: FormBuilder, private httpService: HttpService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadForm();
    this.route.params.subscribe(params => {
      if(params && params.id && params.id > 0) {
        this.httpService.get('Contacts/' + params.id).subscribe((response: any) => {
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
      phoneNumber: new FormControl('', [Validators.required]),
      contactType: new FormControl(0, [Validators.required]),
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
      birthdate: model.birthDate,
      editorData: model.editorData
    });
  }

  onSubmit() {

    if (this.contactForm.invalid) {
      this.markFormGroupTouched(this.contactForm)
      return;
    }

    let data = this.contactForm.value;

    let userId = localStorage.getItem('userId');
    if(!userId) {
      userId = new Date().getTime().toString();
      localStorage.setItem('userId', userId)
    }

    data.userId = userId;
    if(data.id) {

      this.httpService.update('contacts/' + data.id, data).subscribe(() => {
        location.href = '/';
      });
    } else {
      
      this.httpService.post('contacts', data).subscribe(() => {
        location.href = '/';
      });
    }

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

}
