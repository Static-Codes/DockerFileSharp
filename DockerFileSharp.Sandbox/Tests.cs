using DockerFileSharp.Common;
using DockerFileSharp.Instructions;

namespace DockerFileSharp.Sandbox;
public class DockerFileGenerationTest
{
    public static void AddInstructionTest() {
        AddInstruction instruction = new("TestSource", "TestDestination");
        Console.WriteLine(instruction.Build());
    }

    public static void MicrosoftNanoserverExample()
    {
        List<IDockerInstruction> instructions = [];

        instructions.AddRange(
            new FromInstruction(ImageName: "microsoft/nanoserver"),
            new CopyInstruction(Source: "testfile.txt", Destination: @"c:\\"),
            new RunInstruction(Commands: [@"dir c:\"])        
        );

        var fileContents = instructions.Build();
        File.WriteAllText("Dockerfile", fileContents);
        // or async using await File.WriteAllTextAsync()
    }

    public static void CudaExample()
    {
        List<IDockerInstruction> instructions = [];
        instructions.AddRange(
        [
            new FromInstruction("scratch", "model"),
            new AddInstruction("https://huggingface.co/bartowski/Llama-3.2-1B-Instruct-GGUF/resolve/main/Llama-3.2-1B-Instruct-Q4_K_M.gguf", "/model.gguf"),
            
            new FromInstruction("scratch", "prompt"),
            // Manually include the heredoc syntax if your library doesn't wrap it
            new RunInstruction(@"COPY <<EOF prompt.txt
        Q: Generate a list of 10 unique biggest countries by population in JSON with their estimated population in 1900 and 2024. Answer only newline formatted JSON with keys ""country"", ""population_1900"", ""population_2024"" with 10 items.
        A:
        [
            {
        EOF"),

            new FromInstruction("ghcr.io/ggml-org/llama.cpp:full-cuda-b5124"),
            new RunInstruction("--device=nvidia.com/gpu=all --mount=from=model,target=/models --mount=from=prompt,target=/tmp ./llama-cli -m /models/model.gguf -no-cnv -ngl 99 -f /tmp/prompt.txt")
        ]);

        var fileContents = instructions.Build();
        File.WriteAllText("Dockerfile", fileContents);
        // or async using await File.WriteAllTextAsync()
    }
}