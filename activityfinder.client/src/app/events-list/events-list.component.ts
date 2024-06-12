import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ActivityListItem } from '../interfaces/activity';
import { ActivitiesService } from '../services/activities.service';
import { fromEvent, Subscription } from 'rxjs';
import { tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.css']
})
export class EventsListComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['date', 'title', 'address', 'star']; // dodaj inne kolumny w razie potrzeby
  dataSource = new MatTableDataSource<ActivityListItem>();
  states: string[] = [
    'Dolnośląskie',
    'Kujawsko-Pomorskie',
    'Lubelskie',
    'Lubuskie',
    'Łódzkie',
    'Małopolskie',
    'Mazowieckie',
    'Opolskie',
    'Podkarpackie',
    'Podlaskie',
    'Pomorskie',
    'Śląskie',
    'Świętokrzyskie',
    'Warmińsko-Mazurskie',
    'Wielkopolskie',
    'Zachodniopomorskie'
  ];
  selectedState: string = 'Małopolskie'

  filterValue = '';
  //selectedCategory = '';

  @ViewChild(MatPaginator) paginator: MatPaginator = {} as MatPaginator;
  @ViewChild(MatSort) sort: MatSort = {} as MatSort;
  @ViewChild('filterInput') filterInput: any;

  private subscriptions: Subscription[] = [];

  constructor(private activitiesService: ActivitiesService) { }

  ngOnInit() {
    this.loadData();
  }

  ngAfterViewInit() {
    const pageSub = this.paginator.page.pipe(tap(() => this.loadData())).subscribe();
    const subSortPage = this.sort.sortChange.pipe(tap(() => {
      if (this.sort.direction === '') {
        this.sort.active = '';
      }
      this.paginator.pageIndex = 0;
      this.loadData();
    })).subscribe();

    const filterSub = fromEvent(this.filterInput.nativeElement, 'keyup')
      .pipe(
        debounceTime(500),  // Opóźnienie 500ms
        distinctUntilChanged(),  // Emituj tylko, jeśli wartość jest różna od poprzedniej
        tap(() => {
          this.applyFilter();
        })
      ).subscribe();

    this.subscriptions.push(pageSub);
    this.subscriptions.push(subSortPage);
    this.subscriptions.push(filterSub);
  }

  onFilterChange(event: KeyboardEvent) {
    const inputElement = event.target as HTMLInputElement;
    this.filterValue = inputElement.value.trim().toLowerCase();
  }

  applyFilter() {
    this.paginator.pageIndex = 0;
    this.loadData();
  }

  applyStateFilter(state: string) {
    this.selectedState = state;
    this.paginator.pageIndex = 0;
    this.loadData();
  }

  loadData() {
    const pageIndex = this.paginator.pageIndex || 0;
    const pageSize = this.paginator.pageSize || 10;
    const sortField = this.sort.active || '';
    const sortDirection = this.sort.direction || '';

    const filter = this.filterValue;
    const state = this.selectedState;    

    const serviceSub = this.activitiesService.activitiesList(pageIndex, pageSize, sortField, sortDirection, filter, state)
      .subscribe(response => {
      this.dataSource.data = response.data;
      this.paginator.length = response.totalCount;
      });

    this.subscriptions.push(serviceSub);
  }

  ngOnDestroy() {

    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }


}
