import { Component, OnInit } from '@angular/core';
import { ContactModel } from 'src/app/interfaces/contactModel';
import { HttpService } from 'src/app/services/http.service';
import { faHeart } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationsService } from 'angular2-notifications';

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
    pageSize: 5,
    currentUser: '',
    sort: 1,
    total: 0
  };
  reservations: ContactModel[];
  faHeart = faHeart;
  rating = {
    rate: 0,
    max: 5,
    newRating: 0
  };
  reservation: any;

  constructor(private httpService: HttpService, private modalService: NgbModal,
    private notificationService: NotificationsService) { }

  ngOnInit(): void {
    this.paginationModel.currentUser = localStorage.getItem('userId');
    this.loadReservations();
  }

  getRating() {
    return Math.floor(Math.random() * Math.floor(5));
  }

  loadReservations(){
    let url = this.httpService.formatUrl('reservations', this.paginationModel);
    this.httpService.get(url).subscribe((result: any) => {
      this.reservations = result.data;
      this.paginationModel.total = result.total;
    });
  }

  favoriteClick(reservation) {
    let model = {
      reservationId: reservation.id,
      userId: this.httpService.getUserId()
    }

    let url = reservation.isFavorite ? 'Favorite/remove' : 'Favorite';
    this.httpService.post(url, model).subscribe(() => {
      this.loadReservations()
    });
  }

  editReservation(model: ContactModel) {
    console.log(model.birthDate)
  }

  openModal(content, reservation) {
    this.rating.newRating = 0;
    this.reservation = reservation;
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
    }, (reason) => {
    });
  }

  saveRating(content) {
    if(this.rating.newRating === 0) {
      this.notificationService.error('Error', 'Please select rating.', {position: ['top', 'right']}, {position: ['top', 'right']});
      return;
    }

    var model = {
      reservationId: this.reservation.id,
      star: this.rating.newRating,
      userId: this.httpService.getUserId()
    }
    this.httpService.post('Rating', model).subscribe(() => {
      this.loadReservations();
      this.modalService.dismissAll('');
    })
  }

}
