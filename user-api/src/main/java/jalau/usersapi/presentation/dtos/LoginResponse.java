package jalau.usersapi.presentation.dtos;
import lombok.Data;

@Data
public class LoginResponse {
    private String accessToken;
    private String tokenType;
    private String role;
}