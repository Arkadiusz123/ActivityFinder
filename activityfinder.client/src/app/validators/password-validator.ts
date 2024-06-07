import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) {
      return null; // Jeśli pole jest puste, nie wykonuj walidacji
    }

    const hasMinLength = value.length >= 8;
    const hasLowerCase = /[a-z]/.test(value);
    const hasNumber = /\d/.test(value);

    const passwordValid = hasMinLength && hasLowerCase && hasNumber;
    return !passwordValid ? { passwordStrength: true } : null;
  };
}
