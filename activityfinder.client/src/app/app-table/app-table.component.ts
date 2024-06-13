import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-table',
  templateUrl: './app-table.component.html',
  styleUrls: ['./app-table.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppTableComponent implements OnInit, OnDestroy {

  @Input() displayedColumns: ColumnItem[] = [];
  @Input() dataSource: MatTableDataSource<any> = {} as MatTableDataSource<any>;
  @Input() menu: any = {};

  @Output() reloadData = new EventEmitter<any>();

  @ViewChild(MatPaginator) paginator: MatPaginator = {} as MatPaginator;
  @ViewChild(MatSort) sort: MatSort = {} as MatSort;

  private subscriptions: Subscription[] = [];
  columnNames: string[] = [];

  constructor() { }

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

    this.reloadData.emit();
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
