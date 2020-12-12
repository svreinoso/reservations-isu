import { Component, OnInit } from '@angular/core';
import { ContactModel } from 'src/app/interfaces/contactModel';
import { HttpService } from 'src/app/services/http.service';
import { faHeart } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.scss']
})
export class ReservationsComponent implements OnInit {

  sortByOptions = [
    {name: 'By Date Ascending', value: 1},
    {name: 'By Date Descending', value: 2},
    {name: 'By Alphabetic Ascending', value: 3},
    {name: 'By Alphabetic Descending', value: 4},
    {name: 'By Ranking Ascending', value: 5},
    {name: 'By Ranking Descending', value: 6}
  ];
  paginationModel = {
    page: 1,
    pageSize: 10,
    currentUser: ''
  };
  reservations: ContactModel[];
  faHeart = faHeart;
  rating = {
    rate: 2.6,
    max: 5
  };

  constructor(private httpService: HttpService) { }

  ngOnInit(): void {
    this.paginationModel.currentUser = localStorage.getItem('userId');
    if(!this.paginationModel.currentUser) {
      this.paginationModel.currentUser = new Date().getTime().toString();
      localStorage.setItem('userId', this.paginationModel.currentUser)
    }
    this.loadReservations();
  }

  loadReservations(){
    let url = this.httpService.formatUrl('Contacts', this.paginationModel);
    this.httpService.get(url).subscribe((result: any) => {
      console.log(result)
      this.reservations = result.data;
    });
  }

  favoriteClick(reservation) {
    let model = {
      contactId: reservation.id,
      userId: reservation.userId
    }

    let url = reservation.isFavorite ? 'Favorite/remove' : 'Favorite';
    this.httpService.post(url, model).subscribe(() => {
      this.loadReservations()
    });
  }

  editReservation(model: ContactModel) {
    console.log(model.birthDate)
  }

}
