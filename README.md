## INSTALLATION
### Using Docker HUB
The project's image is currently being pushed to a Docker HUB repository through Github Actions, if all you need is to test the result then you can do so by downloading [Docker](https://www.docker.com/products/docker-desktop/).

Then you need to pull the image by opening up a command prompt and typing

```
docker pull ziukristian/pokedexapi

```

After that, the image should begin downloading, then run it using

```
docker run -d -p 8080:8080 -p 8081:8081 ziukristian/pokedexapi

```

The endpoints should then be reachable

GET /pokemon/mewtwo
```
http://localhost:8080/pokemon/mewtwo

```

GET /pokemon/translated/mewtwo
```
http://localhost:8080/pokemon/translated/mewtwo

```

## PRODUCTION CHANGES

- One of the first things I would do in a production enviroment is not use minimal apis but controllers. If the project grows we would need to compartimentalize the functional endpoints which would bring us to an implementation of pseudo-controllers (why use minimals at that point?)

- Another change I would make is to remove the translated endpoint and set a query param on the standard pokemon call, this would make the api deeper and narrower

- Adding caching would really benefit this project

- An argument could be made for the use of classes instead of dicts for the external API response mappings

- As the project grows DTOs would be needed

- Some integration tests would need to be put in place (not really an expert on that side)

- Some of the tests could be bundled into theories to make them easier to edit (maybe?)

- I would also add some type of logging and a way to measure performance

- Based on the business model, I would add authentication and authorization

- Some kind of rate limiting could be introduced
