import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IdeaResponseDto, IdeaRequestDto } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class IdeaService {
  private apiUrl = 'http://localhost:8005/api/v1';

  constructor(private http: HttpClient) {}

  getIdeasByTopic(topicId: string): Observable<IdeaResponseDto[]> {
    return this.http.get<IdeaResponseDto[]>(`${this.apiUrl}/topics/${topicId}/ideas`);
  }

  createIdea(topicId: string, idea: IdeaRequestDto): Observable<IdeaResponseDto> {
    return this.http.post<IdeaResponseDto>(`${this.apiUrl}/topics/${topicId}/ideas`, idea);
  }

  vote(ideaId: string): Observable<IdeaResponseDto> {
    return this.http.post<IdeaResponseDto>(`${this.apiUrl}/ideas/${ideaId}/vote`, {});
  }

  unvote(ideaId: string): Observable<IdeaResponseDto> {
    return this.http.post<IdeaResponseDto>(`${this.apiUrl}/ideas/${ideaId}/unvote`, {});
  }
}