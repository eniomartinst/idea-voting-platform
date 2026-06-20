package jalau.usersapi.presentation.controllers;

import jakarta.validation.Valid;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IUserCommandService;
import jalau.usersapi.presentation.dtos.UserCreateDto;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.dtos.UserUpdateDto;
import jalau.usersapi.presentation.mappers.UserMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.concurrent.CompletableFuture;

/**
 * REST controller responsible for handling user command operations.
 */
@CrossOrigin(origins = "*")
@RestController
@RequestMapping("/api/v1/users")
@RequiredArgsConstructor
public class UserCommandController {
 
	private final IUserCommandService userCommandService;
	private final UserMapper userMapper;
	
	/**
	 * Handles HTTP POST requests to create a new user.
	 * <p>
	 * The request body is validated before being mapped to a domain entity.
	 * The operation is processed asynchronously and returns a response
	 * with HTTP status 201 (Created).
	 *
	 * @param request the user creation request DTO
	 * @return a {@link CompletableFuture} containing the response entity
	 */
	@PostMapping
	public CompletableFuture<ResponseEntity<UserResponseDto>> createUser(
		@Valid @RequestBody UserCreateDto request) {
		User user = userMapper.toDomainEntity(request);
		
		return userCommandService.createUser(user)
				   .thenApply(createdUser -> {
					   UserResponseDto response = userMapper.toResponseDtoEntity(createdUser);
					   return ResponseEntity.status(HttpStatus.CREATED).body(response);
				   });
	}

    @PatchMapping("/{id}")
    public CompletableFuture<ResponseEntity<UserResponseDto>> updateUser(
            @PathVariable String id, 
            @Valid @RequestBody UserUpdateDto request) {
		User user = userMapper.toDomainEntity(id, request);
		
		return userCommandService.updateUser(user)
				   .thenApply(updatedUser -> {
					   if (updatedUser == null) {
						   return ResponseEntity.notFound().build();
					   }
					   
					   UserResponseDto response = userMapper.toResponseDtoEntity(updatedUser);
					   return ResponseEntity.ok(response);
				   });
    }

    @DeleteMapping("/{id}")
    public CompletableFuture<ResponseEntity<Void>> deleteUser(@PathVariable String id) {
        return userCommandService.deleteUser(id)
                .thenApply(v -> ResponseEntity.noContent().build());
    }
}