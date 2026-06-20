package jalau.usersapi.presentation.exceptions;

import jalau.usersapi.core.exception.InvalidUserDataException;
import jalau.usersapi.core.exception.UserNotFoundException;
import jalau.usersapi.core.exception.InvalidUserException;
import org.springframework.context.support.DefaultMessageSourceResolvable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

import java.util.Map;
import java.util.concurrent.CompletionException;

@RestControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(UserNotFoundException.class)
    public ResponseEntity<?> handleUserNotFoundException(UserNotFoundException ex) {
        return ResponseEntity.status(HttpStatus.NOT_FOUND).body(
            Map.of("error", ex.getMessage())
        );
    }
    
    @ExceptionHandler(InvalidUserDataException.class)
    public ResponseEntity<?> handleInvalidUserDataException(InvalidUserDataException ex) {
        return ResponseEntity.badRequest().body(
            Map.of("error", ex.getMessage())
        );
    }
    
    @ExceptionHandler(MethodArgumentNotValidException.class)
    public ResponseEntity<?> handleValidationException(MethodArgumentNotValidException ex) {
        
        String errorMessage = ex.getBindingResult()
                                  .getFieldErrors()
                                  .stream()
                                  .findFirst()
                                  .map(DefaultMessageSourceResolvable::getDefaultMessage)
                                  .orElse("Invalid input");
        
        return ResponseEntity.badRequest().body(
            Map.of("error", errorMessage)
        );
    }
    
    @ExceptionHandler(CompletionException.class)
    public ResponseEntity<?> handleCompletionException(CompletionException ex) {
        Throwable cause = ex.getCause();
        
        if (cause instanceof UserNotFoundException) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND)
                       .body(Map.of("error", cause.getMessage()));
        }
        
        if (cause instanceof InvalidUserDataException) {
            return ResponseEntity.badRequest()
                       .body(Map.of("error", cause.getMessage()));
        }
        
        if (cause instanceof MethodArgumentNotValidException exValidation) {
            return handleValidationException(exValidation);
        }

        if (cause instanceof InvalidUserException) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED)
                    .body(Map.of("error", cause.getMessage()));
        }

        cause.printStackTrace();
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                   .body(Map.of("error", "Internal server error"));
    }
    
    @ExceptionHandler(Exception.class)
    public ResponseEntity<?> handleException(Exception ex) {
        ex.printStackTrace();
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(
            Map.of("error", "Internal server error")
        );
    }

    @ExceptionHandler(InvalidUserException.class)
    public ResponseEntity<?> handleInvalidUserException(InvalidUserException ex) {
        return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(
                Map.of("error", ex.getMessage())
        );
    }
}