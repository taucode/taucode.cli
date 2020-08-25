(defblock :name dummy-block-name :is-top t
	(executor
		:description "Manage the rollout of a resource."
		:usage-samples (
			"kubectl rollout undo deployment/abc"
			"kubectl rollout status daemonset/foo"
			"kubectl rollout history deployment/abc"
			)
	)

	(multi-text
		:classes term
		:values "undo" "status" "history"
		:alias sub-command
		:action argument
		:is-single t
		:description "Sub-command to execute"
		:doc-subst "sub-command")

	(some-text
		:classes path
		:alias directory
		:action argument
		:description "Directory to apply sub-command to"
		:doc-subst "directory")

	(end)
)
