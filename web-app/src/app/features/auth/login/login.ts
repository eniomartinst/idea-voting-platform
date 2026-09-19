import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Auth } from '../../../core/services/auth'; 

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder, 
    private authService: Auth, 
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      (this.authService as Auth & { login: (credentials: unknown) => any }).login(this.loginForm.value).subscribe({
        next: (response: any) => { 
          this.router.navigate(['/dashboard']);
        },
        error: (err: any) => { 
          console.error('Erro no login', err);
          alert('Falha ao fazer login. Verifique suas credenciais.');
        }
      });
    }
  }
}