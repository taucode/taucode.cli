(defblock :name dev-idle-check :is-top t
	(executor
		:executor-name dev-idle-check
		:verb "dev-idle-check"
		:description "Dev idle check"
		:usage-samples (
			"libdev dev-idle-check"
		)
	)

	(opt
		(multi-text
			:classes key
			:values "-d" "--directory"
			:alias directory
			:action key)
		(some-text
			:classes path string
			:action value
			:description "Directory"
			:doc-subst "directory"))

	(end)
)
