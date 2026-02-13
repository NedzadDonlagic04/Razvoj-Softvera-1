import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { FakturaTip } from '../../../../../api-services/fakture/fakture-api.models';
import { ProductCategoriesApiService } from '../../../../../api-services/product-categories/product-categories-api.service';
import { FaktureApiService } from '../../../../../api-services/fakture/fakture-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';

interface Tip {
  id: FakturaTip;
  name: string;
}

interface Kategorija {
  id: number;
  name: string;
}

@Component({
  selector: 'app-faktura-add',
  standalone: false,
  templateUrl: './faktura-add.component.html',
  styleUrl: './faktura-add.component.scss',
})
export class FakturaAddComponent implements OnInit {
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private categoriesApi = inject(ProductCategoriesApiService);
  private faktureApi = inject(FaktureApiService);
  private toaster = inject(ToasterService);

  form: FormGroup;
  isSaving = false;
  isLoading = false;

  tipovi: Tip[] = [
    { id: FakturaTip.Ulazna, name: 'Ulazna' },
    { id: FakturaTip.Izlazna, name: 'Izlazna' },
  ];

  kategorije: Kategorija[] = [];

  constructor() {
    this.form = this.fb.group({
      brojRacuna: [''],
      tip: [''],
      napomena: [''],
      items: this.fb.array([]),
    });

    this.addItem();
    this.addItem();
  }
  ngOnInit(): void {
    this.categoriesApi.list().subscribe({
      next: (response) => {
        this.kategorije = response.items;
      },
    });
  }

  get items(): FormArray {
    return this.form.get('items') as FormArray;
  }

  addItem(): void {
    const itemGroup = this.fb.group({
      kategorijaId: [''],
      proizvod: [''],
      kolicina: [1],
    });
    this.items.push(itemGroup);
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  onCancel(): void {
    this.router.navigate(['/admin/fakture']);
  }

  onSubmit(): void {
    if (this.form.valid) {
      this.faktureApi.create(this.form.value).subscribe({
        next: (fakturaId) => {
          this.toaster.success('Faktura kreirana');
          this.router.navigate(['/admin/fakture']);
        },
        error: () => {
          this.toaster.error('Greska pri kreiranju fakture');
          console.log('Something went wrong with form data:', this.form.value);
          this.router.navigate(['/admin/fakture']);
        },
      });
    }
  }
}
