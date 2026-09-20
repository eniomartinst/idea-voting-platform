import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TopicService } from '../../../core/services/topic';
import { TopicResponseDto } from '../../../core/models/api.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {
  topics: TopicResponseDto[] = [];
  topicForm: FormGroup;
  showCreateForm = false;

  constructor(
    private topicService: TopicService,
    private fb: FormBuilder
  ) {
    this.topicForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5)]],
      description: ['', [Validators.required, Validators.minLength(10)]]
    });
  }

  ngOnInit(): void {
    this.loadTopics();
  }

  loadTopics(): void {
    this.topicService.getTopics().subscribe({
      next: (data: TopicResponseDto[]) => this.topics = data,
      error: (err: any) => console.error('Erro ao carregar tópicos', err)
    });
  }

  toggleCreateForm(): void {
    this.showCreateForm = !this.showCreateForm;
    if (!this.showCreateForm) this.topicForm.reset();
  }

  onSubmitTopic(): void {
    if (this.topicForm.valid) {
      this.topicService.createTopic(this.topicForm.value).subscribe({
        next: () => {
          this.loadTopics();
          this.toggleCreateForm();
        },
        error: (err: any) => console.error('Erro ao criar tópico', err)
      });
    }
  }
}