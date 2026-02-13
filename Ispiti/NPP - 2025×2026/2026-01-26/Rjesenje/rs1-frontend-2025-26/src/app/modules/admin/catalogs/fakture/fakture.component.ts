import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FaktureApiService } from '../../../../api-services/fakture/fakture-api.service';
import {
  ListFaktureQueryDto,
  FakturaTip,
  ListFaktureRequest,
} from '../../../../api-services/fakture/fakture-api.models';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';

@Component({
  selector: 'app-fakture',
  standalone: false,
  templateUrl: './fakture.component.html',
  styleUrl: './fakture.component.scss',
})
export class FaktureComponent
  extends BaseListPagedComponent<ListFaktureQueryDto, ListFaktureRequest>
  implements OnInit
{
  private router = inject(Router);
  private faktureApiService = inject(FaktureApiService);

  displayedColumns: string[] = ['brojRacuna', 'tip', 'datumKreiranja', 'brojStavki'];

  constructor() {
    super();
    this.request = new ListFaktureRequest();
  }

  ngOnInit(): void {
    this.initList();
  }

  protected override loadPagedData(): void {
    this.startLoading();

    this.faktureApiService.list(this.paging.page, this.paging.pageSize).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        console.error('Greška pri učitavanju faktura:', err);
        this.stopLoading();
      },
    });
  }

  onNovaFaktura(): void {
    this.router.navigate(['/admin/fakture/add']);
  }

  getTipString(tip: string): string {
    return tip === FakturaTip[FakturaTip.Ulazna] ? 'ULAZNA' : 'IZLAZNA';
  }

  getTipClass(tip: string): string {
    return tip === FakturaTip[FakturaTip.Ulazna] ? 'ulazna' : 'izlazna';
  }
}
