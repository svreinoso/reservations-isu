import { Component, OnInit, ViewChild } from '@angular/core';
import { ContactModel } from 'src/app/interfaces/contactModel';
import { HttpService } from 'src/app/services/http.service';
import { DataTableDirective } from 'angular-datatables';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit {

  @ViewChild(DataTableDirective, {static: false})
  private datatableElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  contacts: ContactModel[];

  constructor(private httpService: HttpService) { }

  ngOnInit(): void {
    this.loadContacts()
  }

  editContact(contact){
    location.href = 'create-contact/' + contact.id;
  }

  deleteContact(contact, datatableElement) {
    if(confirm('are you sure you want to delete the contact: ' + contact.name)) {
      this.httpService.delete('Contacts/' + contact.id).subscribe(() => {
        datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
          dtInstance.ajax.reload();
        });
      });
    }
  }

  loadContacts() {
    const that = this;

    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 5,
      serverSide: true,
      processing: true,
      ajax: (dataTablesParameters: any, callback) => {
        let url = this.httpService.formatUrl('Contacts', {
          pageSize: dataTablesParameters.length,
          page: dataTablesParameters.start || 1,
          sort: dataTablesParameters.order[0].column + '_' + dataTablesParameters.order[0].dir,
          search: dataTablesParameters.search.value
        });
        that.httpService.get(url).subscribe((response: any) => {
          that.contacts = response.data;
          callback({
            recordsTotal: response.total,
            recordsFiltered: response.total,
            data: []
          });
        });
      },
      columns: [
        { data: 'name' }, 
        { data: 'contactType' }, 
        { data: 'phoneNumber' },
        { data: 'birthDate' },
      ],
      drawCallback: () => {
        document.getElementById('contactTable').style.width = '100%';
      }
    };
  }

}
