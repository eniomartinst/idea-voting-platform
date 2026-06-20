package jalau.usersapi.presentation.dtos;

import jakarta.validation.constraints.Size;
import lombok.Data;

@Data
public class UserUpdateDto {
    
    private String name;
    
    private String login;
    
    @Size(min = 6, message = "Password must be at least 6 characters")
    private String password;
}
