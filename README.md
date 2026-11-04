# Consumer-Driven Contract Testing for Microservices

## [University of Piraeus](https://www.unipi.gr/en/home/) | [Department of Informatics](https://cs.unipi.gr/en/)
**MSc course**: Microservices - AI and Microservices

**Semester**: 2

**Project Completion Year**: 2025

## Project Overview

This project demonstrates **consumer-driven contract testing** between two .NET microservices using [Pact](https://pact.io/) (PactNet). An **Orders** service (the consumer) depends on a **Discounts** service (the provider) to calculate a discount amount based on a customer's rating.

Instead of running both services together in end-to-end tests, the consumer defines the requests it sends and the responses it expects in a **contract** (a pact file). The provider is then verified against that contract on its own. If the provider changes in a way that breaks the consumer, the verification fails before the change reaches production.

## How It Works

1. **Consumer test:** `OrdersSrv-Consumer.Tests` describes the expected interaction with the Discounts service and runs it against a Pact mock server:
    - **Request:** `POST /discount` with `{ "customerRating": 4.1, "amountToDiscount": 0.0 }`
    - **Response:** `200 OK` with `{ "customerRating": 4.1, "amountToDiscount": 0.41 }`

    When the test passes, Pact writes the contract to `OrdersSrv-Consumer.Tests/pact/Orders-Discounts.json`.

2. **Provider:** `DiscountSvc-Provider` exposes the real `POST /discount` endpoint and calculates the discount as `customerRating / 10`.

3. **Provider verification:** `ProviderTests` replays every interaction from the pact file against the running provider and checks that the real responses match the contract.

## Technologies Used

- C# / .NET 8
- ASP.NET Core (Minimal APIs and controllers)
- **PactNet 5:** Consumer-driven contract testing
- **xUnit:** Test framework
- **Swagger / Swashbuckle:** API documentation

## Project Structure

```
ContractTestingExample.sln
/OrderSvc-Consumer           # Orders service (consumer)
/OrdersSrv-Consumer.Tests    # Consumer Pact tests, generate the contract
/DiscountSvc-Provider        # Discounts service (provider), POST /discount
/ProviderTests               # Provider verification against the contract
```

The repository also contains `OrdersService`, `DiscountService` and `DiscountService.Tests`. They are an earlier setup that hosts the provider in-process for verification, and they are not part of the solution file.

## Setup Instructions

### Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run the contract tests

1. Clone the repository:

    ```bash
    git clone https://github.com/ApostolisSiampanis/consumer-driven-contract-testing-microservices.git
    cd consumer-driven-contract-testing-microservices
    ```

2. Run the consumer tests. They generate the pact file:

    ```bash
    dotnet test OrdersSrv-Consumer.Tests
    ```

3. In a second terminal, start the provider on port 5100:

    ```bash
    dotnet run --project DiscountSvc-Provider --urls http://localhost:5100
    ```

4. Verify the provider against the contract:

    ```bash
    dotnet test ProviderTests
    ```

If the provider's response no longer matches what the Orders service expects, step 4 fails and shows which part of the contract was broken.

## Acknowledgments
This project was developed as part of the "Microservices - AI and Microservices" MSc course at the University of Piraeus. Contributions and feedback are always welcome!

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.
