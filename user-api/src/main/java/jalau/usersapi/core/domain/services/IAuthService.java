package com.ideavoting.usersapi.core.domain.services;
import com.ideavoting.usersapi.core.domain.entities.Token;
import com.ideavoting.usersapi.core.domain.entities.User;

public interface IAuthService { Token authenticate(User user); }