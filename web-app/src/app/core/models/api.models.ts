export interface UserDto {
  id: string;
  name: string;
  login: string;
}

export interface TopicResponseDto {
  id: string;
  title: string;
  description: string;
  totalIdeasCount: number;
  createdAt: string;
  createdBy?: UserDto;
}

export interface TopicRequestDto {
  title: string;
  description: string;
}

export interface TopicIdeaDto {
  id: string;
  title: string;
}

export interface IdeaResponseDto {
  id: string;
  content: string;
  votesCount: number;
  votedBy: string[];
  createdAt: string;
  topic: TopicIdeaDto;
  createdBy?: UserDto;
}

export interface IdeaRequestDto {
  content: string;
}