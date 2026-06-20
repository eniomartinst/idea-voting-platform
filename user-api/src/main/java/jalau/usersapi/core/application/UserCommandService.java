package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.core.domain.services.IUserCommandService;
import jalau.usersapi.core.exception.InvalidUserDataException;
import jalau.usersapi.core.exception.UserNotFoundException;
import org.springframework.stereotype.Service;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;

import java.util.concurrent.CompletableFuture;

/**
 * Application service responsible for handling user write operations.
 * <p>
 * This class orchestrates the creation of users by delegating persistence
 * to the {@link IUserRepository}. The operation is executed asynchronously
 * to improve responsiveness at the API layer.
 */
@Service
@RequiredArgsConstructor
@Slf4j
public class UserCommandService implements IUserCommandService {
    
    private final IUserRepository userRepository;
    
    /**
     * Creates a new user asynchronously.
     *
     * @param user the user to be created
     * @return a {@link CompletableFuture} containing the created user
     */
    @Override
    public CompletableFuture<User> createUser(User user) {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Creating user");
            return userRepository.createUser(user);
        });
    }
    
    @Override
    public CompletableFuture<User> updateUser(User user) {
        return CompletableFuture.supplyAsync(() -> {
            User existingUser = userRepository.getUser(user.getId());
            if (existingUser == null) {
                log.warn("User not found");
                throw new UserNotFoundException("User not found");
            }
            
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
            
            return userRepository.updateUser(existingUser);
		});
    }

    @Override
    public CompletableFuture<Void> deleteUser(String id) {
        return CompletableFuture.runAsync(() -> {
            log.info("Attempting to delete user with ID: {}", id);

            User existingUser = userRepository.getUser(id);
            if (existingUser == null) {
                log.warn("User not found with ID: {}", id);
                throw new UserNotFoundException("User not found with ID: " + id);
            }

            userRepository.deleteUser(id);
            log.info("User successfully deleted.");
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
