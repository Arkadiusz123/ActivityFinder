import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ErrorMessagesService {

  constructor() { }

  getErrorMessage(form: FormGroup, controlName: string) {
    const control = form.get(controlName);

    if (control == null) {
      return '';
    }

    if (control.hasError('required'))
    {
      return 'Pole wymagane';
    }
    else if (control.hasError('min'))
    {
      return 'Wartość w nieodpowiednim przedziale';
    }
    else if (control.hasError('pattern'))
    {
      return 'Nieprawidłowy format';
    }
    return '';
  }
}
