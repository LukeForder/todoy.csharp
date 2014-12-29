describe(
	'The test specifications for the ToDoService.',
	function() {
	
		beforeEach(module('todoy'));
		
		var httpBackend, qService, toDoService;
		
		beforeEach(
			inject(
				function(_$httpBackend_, _$q_, _toDoService_) {
					httpBackend = _$httpBackend_;
					qService = _$q_;
					toDoService = _toDoService_;
				}
			)
		);
		
		afterEach(function() {
			httpBackend.verifyNoOutstandingExpectation();
			httpBackend.verifyNoOutstandingRequest();
		}); 
	
	     
		it(
			'has an protocol for adding a new todo.',
			function() {
				var toDoModel = {
					Details: 'To do details'
				};
				
				var id = 'f3a9ccb6-78a6-4cae-82bd-8757d4a4887a',
					details = toDoModel.Details,
					createdDate = '2009-06-15T13:45:30.0000000Z';
				
				var response = {
					Id: id,
					Details: details,
					CreatedDate: createdDate
				};
				
				httpBackend.expectPOST(
					'http://localhost:56749/api/todo',
					{
						Details: toDoModel.Details
					}).
				respond(200, response);
				
				var promise = toDoService.addAsync(toDoModel);
			
				httpBackend.flush();
				
				promise.then(
					function(toDoResponse) {
						expect(toDoResponse).not.toBeNull();
						expect(toDoResponse.Id).toBe(id);
						expect(toDoResponse.Details).toBe(details);
						expect(toDoResponse.CreatedDate).toBe(createdDate);
					},
					function() {
						fail();
					}
				);
				
			}
		);
	}
);