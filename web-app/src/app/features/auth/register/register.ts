import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {
  registerForm: FormGroup;

  constructor(
  private fb: FormBuilder, 
  private authService: AuthService, 
  private router: Router
) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      (this.authService as any).register(this.registerForm.value).subscribe({
        next: (response: any) => { 
          alert('Conta criada com sucesso!');
          this.router.navigate(['/login']);
        },
        error: (err: any) => { 
          console.error('Erro no registro', err);
          alert('Erro ao criar conta.');
        }
      });
    }
  }
}