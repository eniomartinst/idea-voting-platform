import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

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
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      // Aqui fazemos a chamada real para a sua User API
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          // Supondo que a API retorne um token
          if (response.token) {
            this.authService.saveToken(response.token);
          }
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          console.error('Erro no login', err);
          alert('Falha ao fazer login. Verifique suas credenciais.');
        }
      });
    }
  }
}