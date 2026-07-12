![NuGet Version](https://img.shields.io/nuget/v/DockerFileSharp)
![.NET 10](https://img.shields.io/badge/.NET-10.0-blue?logo=dotnet)
![License](https://img.shields.io/github/license/Static-Codes/DockerFileSharp)

# DockerFileSharp

A lightweight, cross-platform, and BuildKit ready C# library for generating Dockerfiles using .NET 10+.

## Platforms Supported
- Windows 10/11
- Linux
- macOS 11+

---


## Usage Examples

### File Handling

```csharp
// ADD: Copying a file with permissions
var add = new AddInstruction(Source: "data.tar.gz", Destination: "/app/", Chmod: "644");

// COPY: Copying from a specific build stage
var copy = new CopyInstruction(Source: "bin/", Destination: "/usr/local/bin/", From: "builder");

```

### Execution and Environment

```csharp
// CMD: Defining default execution
var cmd = new CmdInstruction(new[] { "python3", "app.py" }, UseExecForm: true);

// ENTRYPOINT: Setting the main executable
var entry = new EntryPointInstruction(new[] { "/usr/bin/app" });

// ENV: Setting environment variables
var env = new EnvInstruction(new Dictionary<string, string> { { "NODE_ENV", "production" } });

// RUN: Running commands with chaining
var run = new RunInstruction("apt-get update", "apt-get install -y git");

```

### Metadata and Configuration

```csharp
// FROM: Setting base image
var from = new FromInstruction(ImageName: "ubuntu:22.04", Alias: "base");

// LABEL: Adding metadata
var label = new LabelInstruction(new Dictionary<string, string> { { "version", "1.0" } });

// EXPOSE: Defining ports
var expose = new ExposeInstruction("80", "443/tcp");

```

### System and Lifecycle

```csharp
// SHELL: Overriding the default shell
var shell = new ShellInstruction("/bin/bash", "-c");

// STOPSIGNAL: Changing the exit signal
var signal = new StopSignalInstruction("SIGKILL");

// USER: Switching the execution user
var user = new UserInstruction(User: "appuser", Group: "appgroup");

// WORKDIR: Changing working directory
var workdir = new WorkDirInstruction("/home/appuser/src");

// ONBUILD: Triggering a build action
var onBuild = new OnBuildInstruction(new RunInstruction("echo 'Rebuilding'"));

```

### Additional Examples

#### Reference Example 1
https://docs.docker.com/reference/dockerfile/#from

##### Dockerfile
```docker
FROM microsoft/nanoserver
COPY testfile.txt c:\\
RUN dir c:\
```

##### C# Code
```csharp
public static void MicrosoftNanoserverExample()
{
    List<IDockerInstruction> instructions = [];
    instructions.AddRange(
        new FromInstruction(ImageName: "microsoft/nanoserver"),
        new CopyInstruction(Source: "testfile.txt", Destination: "c:\\"),
        new RunInstruction(Commands: [@"dir c:\"])        
    );
    var fileContents = instructions.Build();
    File.WriteAllText("Dockerfile", fileContents);
    // or async using await File.WriteAllTextAsync()
}
```

#### Reference Example 2
https://docs.docker.com/reference/dockerfile/#example-cuda-powered-llama-inference

##### Dockerfile
```docker
FROM scratch AS model
ADD https://huggingface.co/bartowski/Llama-3.2-1B-Instruct-GGUF/resolve/main/Llama-3.2-1B-Instruct-Q4_K_M.gguf /model.gguf

FROM scratch AS prompt
COPY <<EOF prompt.txt
Q: Generate  a list of 10 unique biggest countries by population in JSON with their estimated poulation in 1900 and 2024. Answer only newline formatted JSON with keys "country", "population_1900", "population_2024" with 10 items.
A:
[
    {

EOF

FROM ghcr.io/ggml-org/llama.cpp:full-cuda-b5124
RUN --device=nvidia.com/gpu=all \
    --mount=from=model,target=/models \
    --mount=from=prompt,target=/tmp \
    ./llama-cli -m /models/model.gguf -no-cnv -ngl 99 -f /tmp/prompt.txt
```

##### C# Code
```csharp
public static void CudaExample()
{
    List<IDockerInstruction> instructions = [];
    instructions.AddRange(
    [
        new FromInstruction("scratch", "model"),
        new AddInstruction("https://huggingface.co/bartowski/Llama-3.2-1B-Instruct-GGUF/resolve/main/Llama-3.2-1B-Instruct-Q4_K_M.gguf", "/model.gguf"),
        
        new FromInstruction("scratch", "prompt"),

        // Manually including the heredoc syntax is required for redirection commands.
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
```

---

## Limitations

### Lack of validation
> When DockerFileSharp was originally created, it served as a component for much larger project. The original implementation of DFS did contain several parsing elements, however, during porting, these elements became a hinderance.

---

## Instructions Reference

| Instruction | Description |
| --- | --- |
| **ADD** | Copies files, directories, or remote URLs into the image. |
| **CMD** | Provides default execution commands for the container. |
| **COPY** | Copies files/directories from the build context into the image. |
| **ENTRYPOINT** | Configures the container to run as an executable. |
| **ENV** | Sets environment variables for subsequent instructions. |
| **EXPOSE** | Informs Docker of ports the container listens on at runtime. |
| **FROM** | Sets the base image for the build stage. |
| **LABEL** | Adds metadata to an image via key-value pairs. |
| **ONBUILD** | Triggers an instruction when the image is used as a base. |
| **RUN** | Executes commands to create a new image layer. |
| **SHELL** | Overrides the default shell used for shell-form commands. |
| **STOPSIGNAL** | Sets the system call signal for exiting the container. |
| **USER** | Sets the username/UID and optional group/GID. |
| **WORKDIR** | Sets the working directory for subsequent instructions. |
---

## Implementation Details

* **Instruction Pattern:** All instructions use the `IDockerInstruction` interface.
* **Build Method:** Calling the `.Build()` method on any of these records returns the standardized Dockerfile string representation.


## AI Usage Disclosure
The codebase itself was developed without the use of AI, however, the documentation was written with AI assistance.