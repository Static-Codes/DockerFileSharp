# Guide to Contributing

## Implementing an IDockerInstruction

- Any implementation(s) of `IDockerInstruction` should be use a `record` type to avoid unintended mutations and/or unexpected behavior within previous `IDockerInstruction` implementations, i.e. [`FromInstruction`](../Instructions/FromInstruction.cs)

- Each implementation of `IDockerInstruction` uses the following syntax:

    ```c#
    namespace DockerFileSharp.Instructions;

    using DockerFileSharp.Common;

    public record NameOfInstruction(args) : IDockerInstruction
    {
        // Each Implementation of IDockerInstruction requires a Build() method. 
        public string Build()
        {
            // The implementation's build action(s) go here.

            // Returning the result from the hypothetical events above.
            return "result";
        }
    }

    ```