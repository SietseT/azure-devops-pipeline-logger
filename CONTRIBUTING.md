# How to contribute

Please read these guidelines before contributing:

 - [Issues and bugs](#issue)
 - [Feature requests](#feature)
 - [Submitting a pull request](#pullrequest)

## <a name="issue"></a> Found an issue?

If you find a bug in the source code or a mistake in the documentation, you can help by submitting an issue to the [GitHub repository][github]. Even better you can submit a Pull Request with a fix.

When submitting an issue please include the following information:

- A description of the issue
- The classes and code related to the issue
- The exception message and stacktrace if an error was thrown
- If possible, please include code that reproduces the issue. GitHub's
[Gist][gist] can be used to share large code samples, or you could [submit a pull request](#pullrequest) with the issue reproduced in a new test

The more information you include about the issue, the more likely it is to be fixed!


## <a name="feature"></a> Want a feature?

You can request a new feature by submitting an issue to the [GitHub repository][github]. Before requesting a feature consider the following:

- Is it supported by Azure DevOps pipelines?
- Does it introduce a breaking change?


## <a name="pullrequest"></a> Submitting a pull request

When submitting a pull request to the [GitHub repository][github] make sure to do the following:

- Check that new and updated code follows the existing code formatting and naming standard
- Run unit tests to ensure no existing functionality has been affected
- Write new unit tests to test your changes. All features and fixed bugs must have tests to verify they work

Read [GitHub Help][pullrequesthelp] for more details about creating pull requests.