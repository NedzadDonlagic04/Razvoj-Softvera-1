import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';
import { ToasterService } from '../../../../core/services/toaster.service';
import { DialogHelperService } from '../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../shared/models/dialog-config.model';
import {
  ListDostavljaciQuery,
  ListDostavljaciQueryDto,
} from '../../../../api-services/dostavljaci/dostavljaci-api.models';
import { DostavljacApiService } from '../../../../api-services/dostavljaci/dostavljaci-api.service';

@Component({
  selector: 'app-dostavljaci',
  standalone: false,
  templateUrl: './dostavljaci.component.html',
  styleUrl: './dostavljaci.component.scss',
})
export class DostavljaciComponent
  extends BaseListPagedComponent<ListDostavljaciQueryDto, ListDostavljaciQuery>
  implements OnInit
{
  private api = inject(DostavljacApiService);
  private router = inject(Router);
  private toaster = inject(ToasterService);
  private dialogHelper = inject(DialogHelperService);

  displayedColumns: string[] = ['name', 'code', 'type', 'isActive', 'actions'];

  constructor() {
    super();
    this.request = new ListDostavljaciQuery();
  }

  ngOnInit(): void {
    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load Dostavljaci');
        console.error('Load Dostavljaci error:', err);
      },
    });
  }

  // === UI Actions ===

  onCreate(): void {
    this.router.navigate(['/admin/dostavljaci/add']);
  }

  onEdit(dostavljac: ListDostavljaciQueryDto): void {
    this.router.navigate(['/admin/dostavljaci', dostavljac.id, 'edit']);
  }

  onDelete(dostavljac: ListDostavljaciQueryDto): void {
    this.dialogHelper.dostavljac.confirmDelete(dostavljac.name).subscribe((result) => {
      if (result && result.button === DialogButton.DELETE) {
        this.performDelete(dostavljac);
      }
    });
  }

  private performDelete(dostavljac: ListDostavljaciQueryDto): void {
    this.startLoading();

    this.api.delete(dostavljac.id).subscribe({
      next: () => {
        this.dialogHelper.dostavljac.showDeleteSuccess().subscribe();
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        this.dialogHelper
          .showError('DIALOGS.TITLES.ERROR', 'Dostavljaci.DIALOGS.ERROR_DELETE')
          .subscribe();

        console.error('Delete product error:', err);
      },
    });
  }

  onSearch(): void {
    this.request.paging.page = 1;
    this.loadPagedData();
  }
}
