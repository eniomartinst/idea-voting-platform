package jalau.usersapi.presentation.exceptions;

import jalau.usersapi.core.exception.InvalidUserDataException;
import jalau.usersapi.core.exception.UserNotFoundException;
import org.junit.jupiter.api.Test;
import org.springframework.core.MethodParameter;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BeanPropertyBindingResult;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.MethodArgumentNotValidException;

import java.util.Map;
import java.util.concurrent.CompletionException;

import static org.junit.jupiter.api.Assertions.assertEquals;

class GlobalExceptionHandlerTest {

    private final GlobalExceptionHandler handler = new GlobalExceptionHandler();

    @Test
    void shouldHandleUserNotFoundException() {
        ResponseEntity<?> response = handler.handleUserNotFoundException(new UserNotFoundException("Not found"));
        assertEquals(HttpStatus.NOT_FOUND, response.getStatusCode());
    }

    @Test
    void shouldHandleInvalidUserDataException() {
        ResponseEntity<?> response = handler.handleInvalidUserDataException(new InvalidUserDataException("Invalid data"));
        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
    }
    
    @Test
    void shouldHandleMethodArgumentNotValidException() {
        // create a "real" minimum FieldError
        FieldError fieldError = new FieldError("objectName", "field", "Invalid input");
        
        // BindingResult with errors
        BindingResult bindingResult = new BeanPropertyBindingResult(new Object(), "objectName");
        bindingResult.addError(fieldError);
        
        // Exception created and handle called
        MethodArgumentNotValidException ex = new MethodArgumentNotValidException(null, bindingResult);
        ResponseEntity<?> response = handler.handleValidationException(ex);
        
        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
        assertEquals(Map.of("error", "Invalid input"), response.getBody());
    }
    
    @Test
    void shouldHandleCompletionExceptionWithUserNotFound() {
        CompletionException ex = new CompletionException(new UserNotFoundException("Not found"));
        ResponseEntity<?> response = handler.handleCompletionException(ex);
        assertEquals(HttpStatus.NOT_FOUND, response.getStatusCode());
    }
    
    @Test
    void shouldHandleCompletionExceptionWithInvalidUserData() {
        CompletionException ex = new CompletionException(new InvalidUserDataException("Invalid data"));
        ResponseEntity<?> response = handler.handleCompletionException(ex);
        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
    }
    
    @Test
    void shouldHandleCompletionExceptionWithValidation() {
        BeanPropertyBindingResult bindingResult = new BeanPropertyBindingResult(new Object(), "objectName");
        bindingResult.addError(new FieldError("objectName", "field", "Invalid input"));
        
        // Minimum mock Spring MethodParameter using a real method from this class
        MethodParameter methodParameter = new MethodParameter(Object.class.getMethods()[0], -1);
        
        MethodArgumentNotValidException validationException =
            new MethodArgumentNotValidException(methodParameter, bindingResult);
        
        // Wrap on CompletionException and call handler
        CompletionException ex = new CompletionException(validationException);
        ResponseEntity<?> response = handler.handleCompletionException(ex);
        
        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
        assertEquals(Map.of("error", "Invalid input"), response.getBody());
    }

    @Test
    void shouldHandleCompletionExceptionWithOtherError() {
        CompletionException ex = new CompletionException(new RuntimeException("Server error"));
        ResponseEntity<?> response = handler.handleCompletionException(ex);
        assertEquals(HttpStatus.INTERNAL_SERVER_ERROR, response.getStatusCode());
    }

    @Test
    void shouldHandleGenericException() {
        ResponseEntity<?> response = handler.handleException(new Exception("Generic error"));
        assertEquals(HttpStatus.INTERNAL_SERVER_ERROR, response.getStatusCode());
    }
}