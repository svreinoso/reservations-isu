import { Component, OnInit } from '@angular/core';
import { ContactModel } from 'src/app/interfaces/contactModel';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit {

  dtOptions: DataTables.Settings = {};
  contacts: ContactModel[];

  constructor(private httpService: HttpService) { }

  ngOnInit(): void {
    this.loadContacts()
  }

  loadContacts() {
    const that = this;

    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 5,
      serverSide: true,
      processing: true,
      ajax: (dataTablesParameters: any, callback) => {
        console.log(dataTablesParameters)
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
