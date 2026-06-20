package jalau.usersapi.presentation.controllers;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * REST controller responsible for health check operations.
 * Used to verify if the API is up and running.
 */
@CrossOrigin(origins = "*")
@RestController
@RequestMapping("/api/v1/health")
public class HealthQueryController {
    
    /**
     * Health check endpoint.
     * Returns HTTP 200 OK with no response body.
     *
     * @return ResponseEntity with status 200 OK
     */
    @GetMapping
    public ResponseEntity<Void> checkHealth() {
        return ResponseEntity.ok().build();
    }
}
