package jalau.usersapi.core.application;

import java.util.List;
import java.util.concurrent.CompletableFuture;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.core.domain.services.IUserQueryService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

/**
 * Implementation of IUserQueryService.
 * Handles asynchronous retrieval of users from the repository.
 */
@Service
@RequiredArgsConstructor
@Slf4j
public class UserQueryService implements IUserQueryService {
    
    /** Repository interface for user data access */
    private final IUserRepository userRepository;
    
    /**
     * Retrieves all users asynchronously.
     *
     * @return a CompletableFuture containing a list of users
     */
    @Override
    public CompletableFuture<List<User>> getUsers() {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Reading users");
            return userRepository.getUsers();
        });
    }
    
    /**
     * Retrieves a single user by ID.
     *
     * @param id the user ID
     * @return the user
     */
    @Override
    public CompletableFuture<User> getUser(String id) {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Reading user");
            User user = userRepository.getUser(id);
            if (user == null) {
                log.error("User not found with ID: {}", id);
                throw new jalau.usersapi.core.exception.UserNotFoundException("User not found with ID: " + id);
            }
            return user;
        });
    }

    @Override
    public CompletableFuture<User> getUserByLogin(String login) {
        return CompletableFuture.supplyAsync(() -> {
            log.info("Reading user by login");
            User user = userRepository.getUserByLogin(login);
            if (user == null) {
                throw new jalau.usersapi.core.exception.UserNotFoundException("User not found with login: " + login);
            }
            return user;
        });
    }
}
