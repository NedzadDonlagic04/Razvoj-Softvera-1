import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseFormComponent } from '../../../../../core/components/base-classes/base-form-component';
import { ToasterService } from '../../../../../core/services/toaster.service';
import { DostavljacFormService } from '../services/dostavljac-form.service';
import {
  CreateDostavljacCommand,
  DostavljacType,
  GetDostavljacByIdQueryDto,
} from '../../../../../api-services/dostavljaci/dostavljaci-api.models';
import { DostavljacApiService } from '../../../../../api-services/dostavljaci/dostavljaci-api.service';

@Component({
  selector: 'app-dostavljaci-add',
  standalone: false,
  templateUrl: './dostavljaci-add.component.html',
  styleUrl: './dostavljaci-add.component.scss',
  providers: [DostavljacFormService],
})
export class DostavljaciAddComponent
  extends BaseFormComponent<GetDostavljacByIdQueryDto>
  implements OnInit
{
  private api = inject(DostavljacApiService);
  private formService = inject(DostavljacFormService);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  tipovi = Object.values(DostavljacType).filter((key) => isNaN(Number(key)));

  ngOnInit(): void {
    this.initForm(false);
  }

  protected loadData(): void {}

  protected save(): void {
    if (this.form.invalid || this.isLoading) {
      return;
    }

    this.startLoading();

    const command: CreateDostavljacCommand = {
      name: this.form.value.name,
      code: this.form.value.code,
      type: this.form.value.type,
      isActive: this.form.value.isActive,
    };

    this.api.create(command).subscribe({
      next: (DostavljacId) => {
        this.stopLoading();
        this.toaster.success('Dostavljac created successfully');
        this.router.navigate(['/admin/dostavljaci']);
      },
      error: (err) => {
        this.stopLoading('Failed to create Dostavljac');
        console.error('Create Dostavljac error:', err);
      },
    });
  }

  protected override initForm(isEdit: boolean): void {
    super.initForm(isEdit);
    this.form = this.formService.createDostavljacForm();
  }

  onCancel(): void {
    this.router.navigate(['/admin/dostavljaci']);
  }

  getErrorMessage(controlName: string): string {
    return this.formService.getErrorMessage(this.form, controlName);
  }
}
