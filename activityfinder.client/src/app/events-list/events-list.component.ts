import { Component, OnInit, ViewChild, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivityListItem } from '../interfaces/activity';
import { ActivitiesService } from '../services/activities.service';
import { fromEvent, Subscription } from 'rxjs';
import { tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AppTableComponent, ColumnItem } from '../app-table/app-table.component';

@Component({
  selector: 'app-events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EventsListComponent implements OnInit, OnDestroy {
  displayedColumns: ColumnItem[] = [
    { name: 'date', display: 'Data' },
    { name: 'title', display: 'Tytuł' },
    { name: 'address', display: 'Adres' },
    { name: 'star', display: '' },
  ];
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

  @ViewChild('filterInput') filterInput: any;
  @ViewChild(AppTableComponent) tableComponent!: AppTableComponent;

  private subscriptions: Subscription[] = [];
  private dataSubsription: Subscription | null = null;

  constructor(private activitiesService: ActivitiesService) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    const filterSub = fromEvent(this.filterInput.nativeElement, 'keyup')
      .pipe(
        debounceTime(500),  // Opóźnienie 500ms
        distinctUntilChanged(),  // Emituj tylko, jeśli wartość jest różna od poprzedniej
        tap(() => {
          this.applyFilter();
        })
      ).subscribe();

    this.subscriptions.push(filterSub);
  }

  onFilterChange(event: KeyboardEvent) {
    const inputElement = event.target as HTMLInputElement;
    this.filterValue = inputElement.value.trim().toLowerCase();
  }

  applyFilter() {
    this.tableComponent.paginator.pageIndex = 0;
    this.loadData();
  }

  applyStateFilter(state: string) {
    this.selectedState = state;
    this.tableComponent.paginator.pageIndex = 0;
    this.loadData();
  }

  loadData() {
    if (this.dataSubsription !== null) {
      this.dataSubsription.unsubscribe();      
    }   

    const pageIndex = this.tableComponent.paginator.pageIndex || 0;
    const pageSize = this.tableComponent.paginator.pageSize || 10;
    const sortField = this.tableComponent.sort.active || '';
    const sortDirection = this.tableComponent.sort.direction || '';

    const filter = this.filterValue;
    const state = this.selectedState;    

    this.dataSubsription = this.activitiesService.activitiesList(pageIndex, pageSize, sortField, sortDirection, filter, state)
      .subscribe(response => {
        this.dataSource.data = response.data;
        this.tableComponent.paginator.length = response.totalCount;
      });

  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }
}
