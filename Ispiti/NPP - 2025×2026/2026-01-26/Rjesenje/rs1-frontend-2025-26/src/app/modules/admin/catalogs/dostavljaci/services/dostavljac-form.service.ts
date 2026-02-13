import { Injectable, inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { GetDostavljacByIdQueryDto } from '../../../../../api-services/dostavljaci/dostavljaci-api.models';

@Injectable()
export class DostavljacFormService {
  private fb = inject(FormBuilder);

  createDostavljacForm(dostavljac?: GetDostavljacByIdQueryDto): FormGroup {
    return this.fb.group({
      name: [dostavljac?.name ?? '', [Validators.required]],
      code: [
        dostavljac?.code ?? '',
        [Validators.required, Validators.minLength(3), Validators.maxLength(3)],
      ],
      type: [dostavljac?.type ?? '', [Validators.required]],
      isActive: [dostavljac?.isActive ?? true, [Validators.required]],
    });
  }

  getErrorMessage(form: FormGroup, controlName: string): string {
    const control = form.get(controlName);
    if (!control || !control.errors || !control.touched) {
      return '';
    }

    const errors = control.errors;

    if (errors['required']) {
      return 'This field is required';
    }
    if (errors['minlength']) {
      return `Minimum ${errors['minlength'].requiredLength} characters required`;
    }
    if (errors['maxlength']) {
      return `Maximum ${errors['maxlength'].requiredLength} characters allowed`;
    }
    if (errors['min']) {
      return `Minimum value is ${errors['min'].min}`;
    }
    if (errors['max']) {
      return `Maximum value is ${errors['max'].max}`;
    }
    if (errors['email']) {
      return 'Invalid email format';
    }

    return 'Invalid value';
  }
}
