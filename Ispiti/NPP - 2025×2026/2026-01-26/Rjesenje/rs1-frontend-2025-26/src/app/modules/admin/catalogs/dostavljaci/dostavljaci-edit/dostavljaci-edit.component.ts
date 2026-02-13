import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DostavljacFormService } from '../services/dostavljac-form.service';
import { BaseFormComponent } from '../../../../../core/components/base-classes/base-form-component';
import { ToasterService } from '../../../../../core/services/toaster.service';
import {
  DostavljacType,
  GetDostavljacByIdQueryDto,
} from '../../../../../api-services/dostavljaci/dostavljaci-api.models';
import { DostavljacApiService } from '../../../../../api-services/dostavljaci/dostavljaci-api.service';

@Component({
  selector: 'app-dostavljaci-edit',
  standalone: false,
  templateUrl: './dostavljaci-edit.component.html',
  styleUrl: './dostavljaci-edit.component.scss',
  providers: [DostavljacFormService],
})
export class DostavljaciEditComponent
  extends BaseFormComponent<GetDostavljacByIdQueryDto>
  implements OnInit
{
  private api = inject(DostavljacApiService);
  private formService = inject(DostavljacFormService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  DostavljacId!: number;
  tipovi = Object.values(DostavljacType).filter((key) => isNaN(Number(key)));

  ngOnInit(): void {
    this.DostavljacId = +this.route.snapshot.params['id'];
    this.initForm(true);
  }

  protected loadData(): void {
    this.startLoading();

    this.api.getById(this.DostavljacId).subscribe({
      next: (Dostavljac) => {
        this.model = Dostavljac;
        this.form = this.formService.createDostavljacForm(Dostavljac);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load Dostavljac');
        this.toaster.error('Dostavljac not found');
        console.error('Load Dostavljac error:', err);
        this.router.navigate(['/admin/dostavljaci']);
      },
    });
  }

  protected save(): void {
    if (this.form.invalid || this.isLoading) {
      return;
    }

    this.startLoading();

    const payload = this.form.getRawValue();

    this.api.update(this.DostavljacId, payload).subscribe({
      next: () => {
        this.stopLoading();
        this.toaster.success('Dostavljac updated successfully');
        this.router.navigate(['/admin/dostavljaci']);
      },
      error: (err) => {
        this.stopLoading('Failed to update Dostavljac');
        console.error('Update Dostavljac error:', err);
      },
    });
  }

  onCancel(): void {
    this.router.navigate(['/admin/dostavljaci']);
  }

  getErrorMessage(controlName: string): string {
    return this.formService.getErrorMessage(this.form, controlName);
  }
}
