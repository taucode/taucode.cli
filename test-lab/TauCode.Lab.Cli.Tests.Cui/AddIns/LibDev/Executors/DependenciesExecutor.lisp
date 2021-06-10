(defblock :name deps :is-top t
	(executor
		:executor-name deps
		:verb "deps"
		:description "Shows dependencies"
		:usage-samples (
			"libdev deps"
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
			:doc-subst "directory")		
	)


	(end)
)
