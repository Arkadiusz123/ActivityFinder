import { ChangeDetectionStrategy, Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthenticateService, PasswordReset } from '../services/authenticate.service';
import { ErrorMessagesService } from '../services/error-messages.service';
import { passwordValidator } from '../validators/password-validator';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ResetPasswordComponent implements OnInit, OnDestroy {
  form!: FormGroup;
  email: string = '';
  token: string = '';

  sub: Subscription | null = null;

  constructor(private fb: FormBuilder, private authenticateService: AuthenticateService, private route: ActivatedRoute,
    public errorMessageService: ErrorMessagesService) {
  }

  ngOnInit(): void {
    this.sub = this.route.queryParamMap.subscribe(params => {
      this.token = decodeURIComponent(params.get('token') || '');
      this.email = decodeURIComponent(params.get('email') || '');
      this.createForm();
    });
  }

  private createForm() {
    this.form = this.fb.group({
      email: [this.email, [Validators.required, Validators.email, Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]],
      token: [this.token, [Validators.required]],
      newPassword: ['', [Validators.required, passwordValidator()]]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const model: PasswordReset = this.form.value;
      this.authenticateService.resetPassword(model);
    }
  }

  ngOnDestroy() {
    if (this.sub !== null) {
      this.sub.unsubscribe();
    }
  }

}
