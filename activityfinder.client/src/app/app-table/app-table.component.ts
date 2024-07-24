import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MenuItem } from '../layout/app.component';

@Component({
  selector: 'app-table',
  templateUrl: './app-table.component.html',
  styleUrls: ['./app-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppTableComponent implements OnInit, OnDestroy {

  @Input() displayedColumns: ColumnItem[] = [];
  @Input() dataSource: MatTableDataSource<any> = {} as MatTableDataSource<any>;
  @Input() currentElementTools: MenuItem[] = [];

  @Output() reloadData = new EventEmitter<any>();
  @Output() reloadElementMenu = new EventEmitter<any>();
  @Output() invokeTool = new EventEmitter<any>();

  @ViewChild(MatPaginator) paginator: MatPaginator = {} as MatPaginator;
  @ViewChild(MatSort) sort: MatSort = {} as MatSort;

  private subscriptions: Subscription[] = [];
  columnNames: string[] = [];

  constructor(private router: Router) { }

  ngOnInit(): void {
    this.columnNames = this.displayedColumns.map(a => a.name);
  }

  ngAfterViewInit() {
    const pageSub = this.paginator.page.pipe(tap(() => this.reloadData.emit())).subscribe();
    const subSortPage = this.sort.sortChange.pipe(tap(() => {
      if (this.sort.direction === '') {
        this.sort.active = '';
      }
      this.paginator.pageIndex = 0;
      this.reloadData.emit();
    })).subscribe();

    this.subscriptions.push(pageSub);
    this.subscriptions.push(subSortPage);
  }

  openMenu(element: any) {
    this.reloadElementMenu.emit(element);
  }

  callAction(route: string, funcName: string, id?: number) {
    if (route) {
      this.router.navigate([route]);
    }
    else if (funcName) {
      this.invokeTool.emit({ action: funcName, id: id });
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

}

export interface ColumnItem {
  name: string;
  display: string;
}
