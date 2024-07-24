import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginRegister } from '../interfaces/login-register';
import { AuthenticateService } from '../services/authenticate.service';
import { ErrorMessagesService } from '../services/error-messages.service';
import { passwordValidator } from '../validators/password-validator';

@Component({
  selector: 'app-login-register',
  templateUrl: './login-register.component.html',
  styleUrls: ['./login-register.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginRegisterComponent implements OnInit {
  form: FormGroup;
  registerForm: boolean = false;

  constructor(private authenticateService: AuthenticateService, private fb: FormBuilder, public errorMessageService: ErrorMessagesService) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, passwordValidator()]],
      email: ['']
    });
  }

  ngOnInit(): void {
  }

  setRegister() {
    this.registerForm = true;

    const emailControl = this.form.get('email')!;
    emailControl.setValidators([Validators.required, Validators.email, Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$") ]);
    emailControl.updateValueAndValidity();
  }

  onSubmit() {
    if (this.form.valid) {
      const user: LoginRegister = this.form.value;
      if (this.registerForm) {
        this.authenticateService.registerUser(user);
      }
      else {
        user.email = '';
        this.authenticateService.loginUser(user);
      }      
    }
  }
}
