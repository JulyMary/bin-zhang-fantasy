package fantasy.jobs.properties;


public final class Resources {

	private Resources()
	{
		
	}

	public static String getInvalidOutputText() {
		return "Output element must set value of \"itemCategory\" or \"propertyName\"";
	}

	public static String getInvalidCallTemplateInputText() {
		return "Input element must set value of  \"itemCategory\" or \"propertyName\"";
	}

	public static String getWhenRequireConditionText() {
		return "'when' instruction requires valid condition.";
	}

	public static String getMemberNotFoundExeptionText() {
		return "%1$s does not contain method %2$s.";
	}

	public static String getDoNotSupportMinusStringText() {
		return "Do not support minus operations on string.";
	}

	public static String getDoNotSupportPlusText() {
		return "Do not support plus/minus operations between %1$s and %2$s.";
	}

	public static String getDoNotSupportDivideExactlyText() {
		return "Do not support divide exactly operation between %1$s and %2$s.";
	}

	public static String getDoNotSupportModulusText() {
		return "Do not support modulus operation between %1$s and %2$s.";
	}

	public static String getDoNotSupportMultDivText() {
		return "Do not support multiple/divide operations between %1$s and %2$s.";
	}

	public static String getDoNotSupportNagetiveText() {
		return "Do not support nagetive operation on %1$s.";
	}

	public static String getConditionNotBoolText() {
		return "Evaluate condition did not return boolean value. Source \"%1$s\", Parsed \"%2$s\".";
	}

	public static String getParseConditionFailedText() {
		return "ParseCoditionFailed. Source \"%1$s\", Parsed \"%2$s\".";
	}

	public static String getTaskFailedText() {
		return "Execute task %1$s failed.";
	}

	public static String getUndefinedTaskOutputParameterText() {
		return "Task %1$s has not output parameter %2$s.";
	}

	public static String getCannotConverTaskOutputParameterToTaskItemText() {
		return "Cannot convert parameter %2$s of task %1$s to TaskItem.";
	}

	public static String getInputParamContainsMultipleItemsText() {
		return "Input parameter %1$s of task %2$s contains %3$d TaskItems.";
	}

	public static String getSetTaskParamErrorText() {
		return "A error occurs when set task %1$s's parameter %2$s.";
	}

	public static String getInlineTaskMemberMustBeElementText() {
		return "Task Member with flag Inline must be type of org.jdom2.Element. Task %1$s, Attribute $2$s.";
	}

	public static String getRequireAttrubiteOfTaskText() {
		return "Require attribute %1$s of task %2$s.";
	}

	public static String getStringParserUndefinedTagWarningText() {
		return "Undefined tag %1$s(%2$s). Using empty string (\"\") as default value.";
	}

	public static String getTargetTermianteText() {
		return "An exception occurs while executing target '%1$s', job is terminating.";
	}

	public static String getTargetContinueText() {
		return "An exception occurs while excuting target '%1$s' and was ignored.";
	}

	public static String getUnitlSuccessFailedText() {
		return "All try elements are failed, until-success is failed.";
	}

	public static String getUntilSuccessAllSkippedText() {
		return "All try elements are skipped, until-success is failed.";
	}

	public static String getMissingResourceNameText() {
		return "Resource expression %1$s missing  name.";
	}

	public static String getUnknownTaskText() {
		return "Unkown task or instruction with uri %1$s and name %2$s.";
	}

	public static String getDulplicateTaskText() {
		return "Tasks %1$s and %2$s have same taks name %3$s %4$s.";
	}

	public static String getUndefinedTargetText() {
		return "Undefined target %1$s.";
	}

	public static String getNoStartupTargetText() {
		return "Must assign target name  because template %1$s has not default target.";
	}

	public static String getInvalidJobTransitionText() {
		return "Job %1$s can not transite from %2$s to %3$s.";
	}

	public static String getDulplicateTemplateNamesText() {
		return "File %1$s and %2$s have same template name %3$s.";
	}

	public static String getSuccessToLoadTemplateText() {
		return "Success to load template file %1$s. Template name is %2$s.";
	}

	public static String getSuccessToLoadAnonymousTemplateText() {
		return "Success to load anonymous template file %1$s.";
	}

	public static String getFailToLoadTemplateText() {
		return "Cannot not load template file %1$s";
	}

	public static String getCannotFindTemplateByNameText() {
		return "Cannot find job template with name '%1$s'.";
	}

	public static String getCannotFindTemplateByPathText() {
		return "Cannot find job template at %1$s.";
	}

	public static String getJobInitializeFailedMessage() {
		return "Job %1$s initialze failed.";
	}

	public static String getJobExitErrorMessage() {
		
		return "A error occurs when job is exiting.";
	}

	public static String getJobFatalErrorMessage() {
	
		return "A fatal error occurs, exit excuting.";
	}

	public static String getJobExitMessage() {
		
		return "Exit with code %1$s.";
	}

	public static String getRetryLaterMessage() {
		
		return "Retry instruction catchs a exception, will try again later.";
	}

	public static String getRetryExceedLimitMessage() {
		
		return "Retry instruction catchs a exception and repeat times exceed maximum number (%1$s).";
	}

	public static String getTaskExecuteErrorMessage() {
		
		return "An error occurs when execute target %1$s.";
	}

	public static String getExecuteOnFailedErrorMessage() {
		
		return "An error occurs when execute onFailed of target %1$s.";
	}

	public static String getTryNextMessage() {

		return "try instruction faild, try next one.";
	}

    public static String getStartJobMessage() {
		
		return "Start Job %1$s (%2$s).";
	}

	public static String getResumeJobMessage() {
		return "Resume Job %1$s (%2$s).";
	}
	
	public static String getLoadJobStartInfoFileMessage() {
		return  "Load job start info at %1$s.";
	}

	public static String getSkipCallTemplateMessage() {
		return "Skip callTemplate %1$s";
	}

	public static String getEvalConditionMessage() {
		return "Source: %1$s, Parsed: %2$s, Value: %3$s.";
	}

	public static String getExecuteTaskMessage() {
		return  "Execute task %1$s.";
	}

	public static String getSkipTaskMessage() {
		return "Skip task %1$s.";
	}

	public static String getStartMessage() {
		return "Start";
	}

	public static String getResumeMessage() {
		return "Resume";
	}

	public static String getSetPropertyMessage() {
		return "set property %1$s as %2$s";
	}

	public static String getExecuteTargetMessage() {
		return "Execute target %1$s.";
	}

	public static String getExecuteTryMessage() {
		return "Execute try No.%1$d";
	}

	public static String getSkipTryMessage() {
		return "Skip try No.%1$d.";
	}
	
}
