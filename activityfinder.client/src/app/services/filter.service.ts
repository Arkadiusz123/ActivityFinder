import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  private filters: any = {};
  private pageIndex: number = 0;
  private pageSize: number = 10;
  private sortActive: string = '';
  private sortDirection: string = '';
  private hasSettings: boolean = false;

  constructor() { }

  setFilters(filters: any): void {
    this.filters = filters;
    this.hasSettings = true;
  }

  getFilters(): any {
    return this.filters;
  }

  setPagination(pageIndex: number, pageSize: number): void {
    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
  }

  getPagination(): { pageIndex: number, pageSize: number } {
    return { pageIndex: this.pageIndex, pageSize: this.pageSize };
  }

  setSort(sortActive: string, sortDirection: string): void {
    this.sortActive = sortActive;
    this.sortDirection = sortDirection;
  }

  getSort(): { sortActive: string, sortDirection: string } {
    return { sortActive: this.sortActive, sortDirection: this.sortDirection };
  }


  settingsSaved(): boolean {
    return this.hasSettings;
  }
}
