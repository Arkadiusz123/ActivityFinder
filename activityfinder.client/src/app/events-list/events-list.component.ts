import { Component, OnInit, ViewChild, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivityListItem } from '../interfaces/activity';
import { ActivitiesPaginationSettings, ActivitiesService } from '../services/activities.service';
import { map, Subscription } from 'rxjs';
import { AppTableComponent, ColumnItem } from '../app-table/app-table.component';
import { MenuItem } from '../layout/app.component';
import { AuthenticateService } from '../services/authenticate.service';

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
    { name: 'usersCount', display: 'Uczestnicy' },
    { name: 'address', display: 'Adres' },
    { name: 'tools', display: '' },
  ];
  dataSource = new MatTableDataSource<ActivityListItem>();
  states: string[] = [
    'dolnośląskie',
    'kujawsko-Pomorskie',
    'lubelskie',
    'lubuskie',
    'łódzkie',
    'małopolskie',
    'mazowieckie',
    'opolskie',
    'podkarpackie',
    'podlaskie',
    'pomorskie',
    'śląskie',
    'świętokrzyskie',
    'warmińsko-mazurskie',
    'wielkopolskie',
    'zachodniopomorskie'
  ];
  selectedState: string = 'małopolskie'

  filterValue: string = '';
  addressInput: string = '';
  selectedStatus = '1';

  currentElementTools: MenuItem[] = [];

  @ViewChild(AppTableComponent) tableComponent!: AppTableComponent;

  isLogged: boolean = false;

  private dataSubscription: Subscription | null = null;
  private loggedSubscription: Subscription | null = null;
  private joinSubscription: Subscription | null = null;

  constructor(private activitiesService: ActivitiesService, private authService: AuthenticateService) { }

  ngOnInit() {
    this.loggedSubscription = this.authService.isLoggedIn().subscribe(x => this.isLogged = x);
  }

  applyFilter() {
    this.tableComponent.paginator.pageIndex = 0;
    this.loadData();
  }

  loadData() {
    if (this.dataSubscription !== null) { this.dataSubscription.unsubscribe(); }

    const settings = {} as ActivitiesPaginationSettings;

    settings.page = (this.tableComponent.paginator.pageIndex || 0) + 1;
    settings.size = this.tableComponent.paginator.pageSize || 10;
    settings.sortField = this.tableComponent.sort.active || 'date';
    settings.asc = this.tableComponent.sort.direction == 'asc';
    settings.address = this.addressInput;
    settings.state = this.selectedState;
    settings.status = +this.selectedStatus;

    this.dataSubscription = this.activitiesService.activitiesList(settings).pipe(
      map(response => ({
        total: response.totalCount,
        objects: response.data.map(obj => this.addCalculatedValue(obj))
      }))
    )
      .subscribe(response => {
        this.dataSource.data = response.objects;
        this.tableComponent.paginator.length = response.total;
      });
  }

  reloadElementMenu(element: ActivityListItem) {
    const items: MenuItem[] = [];

    if (element.createdByUser) {
      items.push({ route: '/event-form/' + element.id, display: 'Edytuj', clickAction: '' } as MenuItem);
    }
    if (this.isLogged && !element.alreadyJoined) {
      items.push({ route: '', display: 'Dołącz', clickAction: 'join', id: element.id } as MenuItem);
    }

    this.currentElementTools = items;
  }

  invokeTool(data: { action: string, id?: number }) {
    if (data.action === 'join') { this.joinEvent(data.id!); }
  }

  joinEvent(id: number) {
    if (this.joinSubscription !== null) { this.joinSubscription.unsubscribe(); }
    this.joinSubscription = this.activitiesService.joinActivity(id).subscribe(x => this.loadData());
  }

  addCalculatedValue(obj: ActivityListItem) {
    return {
      ...obj,
      usersCount: obj.joinedUsers + (obj.usersLimit ? `/${obj.usersLimit}` : '')
    };
  }

  ngOnDestroy() {
    if (this.dataSubscription !== null) { this.dataSubscription.unsubscribe(); }
    if (this.loggedSubscription !== null) { this.loggedSubscription.unsubscribe(); }
    if (this.joinSubscription !== null) { this.joinSubscription.unsubscribe(); }
  }
}
