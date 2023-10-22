# Overview of My Automation Testing Project with Selenium


In this project, We created an automated user interface (UI) testing script to test against DEP's Idling Complaints website. 

## Tools for Automated UI Testing

* Selenium WebDriver
	* Web framework that allows browser test executions
	
* Microsoft Visual Studio 2022
	* Integrated Development Environment (IDE) for .NET and C++ development on Windows
	* C# (programming language) is used for this project 

* NUnit
	* Unit-Testing framework for all .NET languages


### Project Folder Structure Map

<img height="500" src="https://github.com/Tiffany678/NYCIdlingComplaints/blob/master/Structure.png" alt="Get request" width="650"/>























## The REST Controller
Now let’s implement the REST API. In this case, it’s just a simple REST controller:

```
@RestController
public class CakeController {

    // standard constructors
    
	private CakeService cakeService;

	@PostMapping("/cake")
	public Cake createCake(@RequestBody Cake cake){
		return cakeService.create(cake);
	}
	
	//Some other operation methods
}
```

## The Junit test cases
Finally, let’s test our API endpoint with RestTemplate:

```
public class CakeTests {
    
    @Test
    void testGetACake() {
        RestTemplate restTemplate = new RestTemplate();

        Cake cake= restTemplate.getForObject("http://localhost:8080/cake/{id}", Cake.class, 1);

        System.out.println(cake.getTitle());
    }
	
	//Some other Junit test cases
}
```

## Showing Some API endpoint testing result by Postman
After running our application, We can also view our data with Postman tool

Test the get all request:

<img height="500" src="https://github.com/Tiffany678/BakeryShop_PassionProject/blob/main/Images/PostmanGet.png" alt="Get request" width="650"/>

Test the get item by id request:

<img height="500" src="https://github.com/Tiffany678/BakeryShop_PassionProject/blob/main/Images/PostmanGetById.png" alt="Get request" width="650"/>

Test the Post request:

<img height="500" src="https://github.com/Tiffany678/BakeryShop_PassionProject/blob/main/Images/PostmanPostMethod.png" alt="Get request" width="650"/>

<img height="500" src="https://github.com/Tiffany678/BakeryShop_PassionProject/blob/main/Images/PostmanTestPost.png" alt="Get request" width="650"/>


## Conclusion
In this article, we learned how to build a basic web application with Spring Boot.
