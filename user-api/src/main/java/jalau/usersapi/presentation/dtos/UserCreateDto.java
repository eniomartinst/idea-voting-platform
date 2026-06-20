package jalau.usersapi.presentation.dtos;

import jakarta.validation.constraints.NotBlank;
import lombok.Data;

/**
 * Data Transfer Object used to receive user creation requests.
 * <p>
 * Contains validation rules to ensure required fields are provided.
 */
@Data
public class UserCreateDto {
    
    /**
     * The user's full name.
     * Must not be blank.
     */
    @NotBlank(message = "Name cannot be empty")
    private String name;
    
    /**
     * The user's login identifier.
     * Must not be blank.
     */
    @NotBlank(message = "Login cannot be empty")
    private String login;
    
    /**
     * The user's password.
     * Must not be blank.
     */
    @NotBlank(message = "Password cannot be empty")
    private String password;
}
