package com.ideavoting.usersapi.services;

import com.ideavoting.usersapi.exceptions.InvalidUserDataException;
import com.ideavoting.usersapi.exceptions.UserNotFoundException;
import com.ideavoting.usersapi.models.User;
import com.ideavoting.usersapi.repositories.UserRepository;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.concurrent.CompletableFuture;

@Service
@RequiredArgsConstructor
@Slf4j
public class UserService {

    private final UserRepository userRepository;

    public CompletableFuture<User> createUser(User user) {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Creating user");
            return userRepository.save(user);
        });
    }

    public CompletableFuture<User> updateUser(User user) {
        return CompletableFuture.supplyAsync(() -> {
            User existingUser = userRepository.findById(user.getId())
                    .orElseThrow(() -> {
                        log.warn("User not found");
                        return new UserNotFoundException("User not found");
                    });

            if (user.getName() == null && user.getLogin() == null && user.getPassword() == null) {
                log.warn("Empty PATCH request - no fields to update");
                throw new InvalidUserDataException("At least one field must be provided for update");
            }
            log.info("Updating user with ID: {}", user.getId());

            String name = validateAndNormalize(user.getName(), "name");
            if (name != null) existingUser.setName(name);

            String login = validateAndNormalize(user.getLogin(), "login");
            if (login != null) existingUser.setLogin(login);

            String password = validateAndNormalize(user.getPassword(), "password");
            if (password != null) existingUser.setPassword(password);

            return userRepository.save(existingUser);
        });
    }

    public CompletableFuture<Void> deleteUser(String id) {
        return CompletableFuture.runAsync(() -> {
            log.info("Attempting to delete user with ID: {}", id);

            if (!userRepository.existsById(id)) {
                log.warn("User not found with ID: {}", id);
                throw new UserNotFoundException("User not found with ID: " + id);
            }

            userRepository.deleteById(id);
            log.info("User successfully deleted.");
        });
    }

    public CompletableFuture<List<User>> getUsers() {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Reading users");
            return userRepository.findAll();
        });
    }

    public CompletableFuture<User> getUser(String id) {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Reading user");
            return userRepository.findById(id)
                    .orElseThrow(() -> {
                        log.error("User not found with ID: {}", id);
                        return new UserNotFoundException("User not found with ID: " + id);
                    });
        });
    }

    public CompletableFuture<User> getUserByLogin(String login) {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Reading user by login");
            return userRepository.findByLogin(login)
                    .orElseThrow(() -> new UserNotFoundException("User not found with login: " + login));
        });
    }

    private String validateAndNormalize(String value, String fieldName) {
        if (value == null) return null;

        String normalized = value.trim();
        if (normalized.isEmpty()) {
            log.warn("Invalid {}", fieldName);
            throw new InvalidUserDataException("Invalid " + fieldName);
        }
        return normalized;
    }
}