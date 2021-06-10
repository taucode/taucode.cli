(defblock :name change-directory :is-top t
	(executor
		:executor-name change-directory
		:verb "cd"
		:description "Changes current directory"
		:usage-samples (
			"libdev cd c:\work"
		)
	)

	(opt
		(some-text
			:classes path string
			:alias directory
			:action argument		
			:description "Directory to set as current"
			:doc-subst "directory")
	)

	(end)
)
