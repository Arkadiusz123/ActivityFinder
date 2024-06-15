import { Component, OnInit, ViewChild, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivityListItem } from '../interfaces/activity';
import { ActivitiesService } from '../services/activities.service';
import { Subscription } from 'rxjs';
import { AppTableComponent, ColumnItem } from '../app-table/app-table.component';
import { MenuItem } from '../layout/app.component';

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
    { name: 'tools', display: '' },
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

  filterValue: string = '';
  addressInput: string = '';
  selectedStatus: number = 1;

  currentElementTools: MenuItem[] = [];

  @ViewChild(AppTableComponent) tableComponent!: AppTableComponent;

  private dataSubsription: Subscription | null = null;

  constructor(private activitiesService: ActivitiesService) { }

  ngOnInit() {
  }

  applyFilter() {
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

    this.dataSubsription = this.activitiesService.activitiesList(pageIndex, pageSize, sortField, sortDirection, this.addressInput, this.selectedState)
      .subscribe(response => {
        this.dataSource.data = response.data;
        this.tableComponent.paginator.length = response.totalCount;
      });
  }

  reloadElementMenu(element: ActivityListItem) {
    const items: MenuItem[] = [];
    items.push({ route: '/authenticate', display: 'Strona główna', clickAction: '' } as MenuItem);

    this.currentElementTools = items;
  }

  ngOnDestroy() {
  }
}
