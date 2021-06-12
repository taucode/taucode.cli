(defblock :name start-release :is-top t
	(executor
		:executor-name start-release
		:verb "start-release"
		:description "Starts new release"
		:usage-samples (
			"libdev start-release -v 1.2.3 -d c:\work\taucode.foo"
			"libdev start-release"
		)
	)

	(opt
		(multi-text
			:classes key
			:values "-v" "--version"
			:alias version
			:action key)
		(some-text
			:classes path string ; TODO: class should be 'version'
			:action value
			:description "Version"
			:doc-subst "version"))

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
