describe(
	"Unit Tests for the authentication service.",
	function() {
		
		function createPasswordValidationRequest(httpBackend) {
			return httpBackend.
				expectPOST( 
					'http://localhost:56749/api/login',
					{
						EmailAddress: 'foo',
						Password: 'bar'
					}
				);
		}
		
		function createRegistrationRequest(httpBackend, registrationDetails) {
			return httpBackend.
				expectPOST( 
					'http://localhost:56749/api/register',
					{
						EmailAddress: registrationDetails.emailAddress,
						Password: registrationDetails.password,
						PasswordConfirmation: registrationDetails.passwordConfirmation 
					}
				);
		}
		
	beforeEach(module('todoy'));
	
	var httpBackend, qService, authenticationService;
	
	beforeEach(
		inject(function(_$httpBackend_, _$q_, _authenticationService_) {
			httpBackend = _$httpBackend_;
			qService = _$q_;
			authenticationService = _authenticationService_;
		})
	);
	
	afterEach(function() {
		httpBackend.verifyNoOutstandingExpectation();
		httpBackend.verifyNoOutstandingRequest();
	}); 
	
	it(
		"rejects an invalid username/password combination",
		function() {
			
			createPasswordValidationRequest(httpBackend).respond(401, '');
			
			var promise = authenticationService.validateCredentialsAsync('foo', 'bar');
			
			httpBackend.flush();
			
			promise.then(
				function() { 		
					fail('authentication should fail');
				}, 
				function(errors) {
					expect(errors).toContain('Invalid user name and password combination.');
				});
		}
	);
	
	it (
		"sets the current users authentication token when a valid username/password is received",
		function() {
			var token = "authenticationToken";
		
			createPasswordValidationRequest(httpBackend).respond(token, {});
			
			var promise =  authenticationService.validateCredentialsAsync('foo', 'bar');
			
			httpBackend.flush();
			
			promise.then(
				function() {
					var currentUser = authenticationService.getUser();
					expect(currentUser).not.toBeNull();
					expect(currentUser.token).toBe(token);
				}, 
				function(errors) {
					fail('authentication should succeed');
				});
			
		}
	);
	
	
	it (
		"sets the error reasons when the server when the server returns an error response",
		function() {
			var token = "authenticationToken",
				
			errorMessage = 'An error occured logging in! :(';
		
			createPasswordValidationRequest(httpBackend).respond(500, {Errors: [errorMessage] });
			
			var promise =  authenticationService.validateCredentialsAsync('foo', 'bar');
			
			httpBackend.flush();
			
			promise.then(
				function() {
					fail('authentication should fail');
				}, 
				function(errors) {
						expect(errors).toContain(errorMessage);
				});
			
		}
	);
	
	it (
		"allows user registration when the registration details are valid",
		function() {			
		
			var registrationDetails = new todoy.authentication.models.RegistrationDetails('test@testing.com', 'aSup3rSecretPassword!','aSup3rSecretPassword!');
			
			createRegistrationRequest(httpBackend, registrationDetails).respond(200, '');
			
			var promise =  authenticationService.registerAsync(registrationDetails);
			
			httpBackend.flush();
			
			promise.then(
				function() {
					// the resolve returns no data.
				}, 
				function(errors) {
						fail();
				});
			
		}
	);
	
	it (
		"user registration fails when the registration details are invalid.",
		function() {			
			var errorMessage = 'Email Address must be provided';
		
			var registrationDetails = new todoy.authentication.models.RegistrationDetails(null,'aSup3rSecretPassword!', 'aSup3rSecretPassword!');
		
			createRegistrationRequest(httpBackend, registrationDetails).respond(400, [errorMessage]);
			
			var promise =  authenticationService.registerAsync(registrationDetails);
			
			httpBackend.flush();
			
			promise.then(
				function() {
					fail();
				}, 
				function(errors) {
					expect(errors).toContain(errorMessage);
				});
			
		}
	);
	

	
});