import { Component, OnInit, ViewChild, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivityListItem } from '../interfaces/activity';
import { ActivitiesPaginationSettings, ActivitiesService } from '../services/activities.service';
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
  selectedStatus = '1';

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

    const settings = {} as ActivitiesPaginationSettings;

    settings.page = (this.tableComponent.paginator.pageIndex || 0) + 1;
    settings.size = this.tableComponent.paginator.pageSize || 10;
    settings.sortField = this.tableComponent.sort.active || 'date';
    settings.asc = this.tableComponent.sort.direction == 'asc';
    settings.address = this.addressInput;
    settings.state = this.selectedState;
    settings.status = +this.selectedStatus;

    this.dataSubsription = this.activitiesService.activitiesList(settings)
      .subscribe(response => {
        this.dataSource.data = response.data;
        this.tableComponent.paginator.length = response.totalCount;
      });
  }

  reloadElementMenu(element: ActivityListItem) {
    const items: MenuItem[] = [];

    if (element.createdByUser) {
      items.push({ route: '/event-form/' + element.id, display: 'Edytuj', clickAction: '' } as MenuItem);
    }

    this.currentElementTools = items;
  }

  ngOnDestroy() {
    if (this.dataSubsription !== null) {
      this.dataSubsription.unsubscribe();
    }
  }
}
